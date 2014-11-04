CurrentDir = $(CURDIR)
Source = $(CurrentDir)/src/xmindKeygen/Generator.java $(CurrentDir)/src/xmindKeygen/Injector.java $(CurrentDir)/src/xmindKeygen/Main.java
Libraries = $(CurrentDir)/../../lib/*

Java = java
Javac = javac
Jar = jar
Copy = cp
JavaArchiver = jar
Makedir = mkdir -p


TargetDir = $(CurrentDir)/build
OrigDir = $(CurrentDir)/../../bak
OrigLib = $(OrigDir)/*

ClassPath = $(Libraries)
CompileClassPathOptions = -classpath "$(ClassPath)"
OutputOptions = -d "$(TargetDir)"
JavaCompilerOptions = $(CompileClassPathOptions) $(OutputOptions) $(Source)
MainClass = xmindKeygen.Main
RunClassPath = "$(ClassPath):$(OrigLib):."
RunClassPathOptions = -classpath $(RunClassPath)
UpdateJarOption1 = -uf net.xmind.verify_3.5.0.201410310637.jar net/
UpdateJarOption2 = -uf org.xmind.meggy_3.5.0.201410310637.jar org/
UpdateJarCommand = $(Jar) $(UpdateJarOption1);$(Jar) $(UpdateJarOption2);
ChangeDirCommand = cd $(TargetDir) 

new:
	$(Makedir) $(TargetDir)
build:new
	$(Javac) $(JavaCompilerOptions)

run: build
	$(ChangeDirCommand) && $(Java) $(RunClassPathOptions) $(MainClass)
	$(Copy) $(OrigDir)/*.jar $(TargetDir)
	$(ChangeDirCommand) && $(UpdateJarCommand) 

clean:
	rm -rf $(TargetDir)
default: build
