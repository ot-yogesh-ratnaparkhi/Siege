using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Siege.Requisitions.Registrations;

namespace Siege.Requisitions.AutoMocker
{
    public class Mock<TargetClass>
    {
        public static List<IRegistration> Using(IMockAdapter adapter)
        {
            AutoMocker mocker = new AutoMocker(adapter, typeof(TargetClass));

            var registrations = new List<IRegistration>();
            mocker.Register(typeof(TargetClass), registrations);

            return registrations;
        }

        public static List<IRegistration> Using(IMockAdapter adapter, IServiceLocator serviceLocator)
        {
            AutoMocker mocker = new AutoMocker(serviceLocator, adapter, typeof(TargetClass));

            var registrations = new List<IRegistration>();
            mocker.Register(typeof(TargetClass), registrations);

            return registrations;
        }

        //Moved the logics to a nested class to get rid of the static variables
        class AutoMocker
        {
            private readonly IMockAdapter mockAdapter;
            private readonly Type baseType;
            private readonly IServiceLocator serviceLocator;
            public AutoMocker(IServiceLocator servicelocator, IMockAdapter adapter, Type baseType)
            {
                mockAdapter = adapter;
                this.baseType = baseType;
                this.serviceLocator = servicelocator;
            }

            public AutoMocker(IMockAdapter adapter, Type baseType)
                : this(null, adapter, baseType)
            {
            }

            public object Register(Type type, IList<IRegistration> registrations)
            {
                if (serviceLocator != null)
                {
                    try
                    {
                        var slInstance = serviceLocator.GetInstance(type);
                        if (slInstance != null)
                            return slInstance;
                    }
                    catch
                    {
                        //This is to ignore the registration not found exceptions
                    }
                }

                if (type.IsPrimitive || type == typeof(string) || type.IsEnum)
                {
                    object speicalCases = type == typeof(string) ? string.Empty : Activator.CreateInstance(type);
                    return speicalCases;
                }
                ConstructorInfo constructor = SelectConstructor(type);
                if (constructor == null)
                {
                    var instance = mockAdapter.Mock(type, new object[0]);
                    registrations.Add(new AutoMockRegistration(type, instance));
                    return instance;
                }
                var constructionParameters = new List<object>();
                if (constructor.GetParameters().Count() > 0)
                {
                    foreach (ParameterInfo info in constructor.GetParameters())
                    {
                        var instance = Register(info.ParameterType, registrations);
                        constructionParameters.Add(instance);
                    }
                }
                object stubbedInstance = (type == baseType || type.IsValueType)
                    ? constructor.Invoke(constructionParameters.ToArray())
                    : mockAdapter.Stub(type, constructionParameters.ToArray());
                registrations.Add(new AutoMockRegistration(type, stubbedInstance));
                return stubbedInstance;
            }

            private static ConstructorInfo SelectConstructor(Type type)
            {
                ConstructorInfo[] constructorInfos = type.GetConstructors();
                if (constructorInfos.Length == 0)
                    return null;

                int maxNumberOfArg = constructorInfos.Max(c => c.GetParameters().Count());
                var constructor = constructorInfos.Where(c => c.GetParameters().Count() == maxNumberOfArg).FirstOrDefault();

                if (constructor == null)
                    throw new ApplicationException(string.Format("AutoMocker cannot find the correct constructor for type {0}", type.Name));

                return constructor;
            }

        }
    }
}
