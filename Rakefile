require 'rubygems'
require 'albacore'

ABSOLUTE_PATH = File.expand_path(File.dirname(__FILE__))
CLR_VERSION = 'v4.0.30319'
FRAMEWORK_DIR = File.join(ENV['windir'].dup, 'Microsoft.NET', 'Framework', CLR_VERSION)
MSBUILD_EXE = File.join(FRAMEWORK_DIR, 'msbuild.exe')
COMPILE_TARGET = 'Release'
SLN_FILE = File.join(ABSOLUTE_PATH,'Siege.ServiceLocator','Siege.ServiceLocator.sln')


desc "compiles"
task :default => [:compile]

################
#  COMPILE THE CODE
################

desc "Compiles the app"
msbuild :compile do |msb|
  msb.command = MSBUILD_EXE
  msb.properties :configuration => COMPILE_TARGET
  msb.targets :Rebuild
  msb.verbosity = "minimal"
  msb.solution = SLN_FILE
end
