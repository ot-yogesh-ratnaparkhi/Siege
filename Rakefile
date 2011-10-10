require 'rubygems'
require 'albacore'

ABSOLUTE_PATH = File.expand_path(File.dirname(__FILE__))
CLR_VERSION = 'v4.0.30319'
FRAMEWORK_DIR = File.join(ENV['windir'].dup, 'Microsoft.NET', 'Framework', CLR_VERSION)
MSBUILD_EXE = File.join(FRAMEWORK_DIR, 'msbuild.exe')
COMPILE_TARGET = 'Release'

desc "compiles"
task :default => [:compile_all]

################
#  COMPILE THE CODE
################

desc "Compile all solutions"
task :compile_all do
  Rake::Task[:compile].execute({ :solution => 'Siege.Futures' }) 
  Rake::Task[:compile].execute({ :solution => 'Siege.Proxy' }) 
  Rake::Task[:compile].execute({ :solution => 'Siege.Repository' }) 
  Rake::Task[:compile].execute({ :solution => 'Siege.ServiceLocator' }) 
  Rake::Task[:compile].execute({ :solution => 'Siege.TypeGenerator' }) 
end

desc "Compiles a single solution"
msbuild :compile, :solution do |msb, args|
  sln_file = File.join(ABSOLUTE_PATH, args[:solution],"#{args[:solution]}.sln")
  puts "Compiling solution #{sln_file}"
  msb.command = MSBUILD_EXE
  msb.properties :configuration => COMPILE_TARGET
  msb.targets :Rebuild
  msb.verbosity = "minimal"
  msb.solution = sln_file
end
