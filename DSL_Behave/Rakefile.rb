require 'rake/clean'
require 'FileUtils'

BuildNumber = "#{ENV['BuildNumber']}"
MsBuildPath = "#{ENV['SystemRoot']}\\Microsoft.NET\\Framework\\v4.0.30319\\msbuild.exe"

UnitTestDll = "BehaviorTests.dll Behavior.Common.Tests.dll"
IntegrationTestDll = ""
AcceptanceTestDll = ""

TestCmd = "\".\\..\\SharedLibs\\NUnit\\nunit-console-x86.exe\""

desc "Default local build task with tests."
task :default => ["build:all"]

namespace :build do
  
  task :all => [:clean, :setupvars, :printvars, :build, :copytests, :unit, :copytobuild]
  
  desc "Setup directory variables"
  task :setupvars do
    
    @currentDir = pwd
    @currentDir.gsub!("/", "\\")
    
    cd ".."
    @workspaceDir = pwd
    @workspaceDir.gsub!("/", "\\")
    
    cd "#{@currentDir}"
    @solutionFile = File.expand_path("#{@currentDir}\\Behave.sln").gsub!("/", "\\")
    
    if File.exists?(".\\Test")
      FileUtils.rm_rf(".\\Test")
    end
    
    if File.exists?(".\\Build")
      FileUtils.rm_rf(".\\Build")
    end
     
    puts "*****************************"
  end
    
  desc "Output current variable values."
  task :printvars do
    puts "currentDir = #{@currentDir}"
    puts "workspaceDir = #{@workspaceDir}"
    puts "solutionFile = #{@solutionFile}"
    puts "BuildNumber = #{BuildNumber}"
    puts "MsBuildPath = #{MsBuildPath}"
    puts "UnitTestDll = #{UnitTestDll}"
    puts "IntegrationTestDll = #{IntegrationTestDll}"
    puts "AcceptanceTestDll = #{AcceptanceTestDll}"
    puts "TestCmd = #{TestCmd}"
    puts "*****************************"
  end
  
  desc "Build the single solution file."
	task :build do
    
		puts "BuildNumber = #{BuildNumber}"
    
		sh "#{MsBuildPath} /p:BuildConfiguration=Debug /p:Platform=x86 #{@solutionFile}"
    
	end
  
  desc "Copy tests assemblies to Test folder"
  task :copytests do
    
    cd @currentDir
    
    mkdir(".\\Test")
    
    cd "#{@currentDir}\\Test"
    
    sh "xcopy /y /e #{@currentDir}\\Behavior.Common.Tests\\bin\\x86\\Debug #{@currentDir}\\Test"
    
    sh "xcopy /y /e #{@currentDir}\\BehaviorTests\\bin\\Debug #{@currentDir}\\Test"
    
  end
  
  task :unit do
    
    cd "#{@currentDir}\\Test"
    puts pwd
    
		puts "Executing unit tests..."
    
		sh "#{TestCmd} #{UnitTestDll}"
    
	end
  
  task :integration do
    
    cd "#{@currentDir}\\Test"
    
		puts "Executing integration tests..."
    
		sh "#{TestCmd} #{UnitTestDll}"
    
	end
  
  task :acceptance do
    
    cd "#{@currentDir}\\Test"
    
		puts "Executing acceptance tests..."
    
		sh "#{TestCmd} #{UnitTestDll}"
    
	end
  
  task :copytobuild do
    
    cd @currentDir
    
    mkdir(".\\Build")
    
    sh "xcopy /y /e #{@currentDir}\\Behavior\\bin\\Debug #{@currentDir}\\Build"
    
    sh "xcopy /y /e #{@currentDir}\\Behavior.Common\\bin\\Debug #{@currentDir}\\Build"
    
    sh "xcopy /y /e #{@currentDir}\\Behavior.Logging\\bin\\Debug #{@currentDir}\\Build"
    
    sh "xcopy /y /e #{@currentDir}\\Behavior.Remote\\bin\\Debug #{@currentDir}\\Build"
    
    sh "xcopy /y /e #{@currentDir}\\Behavior.Reports\\bin\\Debug #{@currentDir}\\Build"
    
    sh "xcopy /y /e #{@currentDir}\\FixtureLauncher\\bin\\Debug #{@currentDir}\\Build"
    
    sh "xcopy /y /e #{@currentDir}\\FixtureServer\\bin\\Debug #{@currentDir}\\Build"
    
  end
  
end