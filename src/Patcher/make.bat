mkdir build

javac -classpath "../../lib/*;../../bak/*" -d "./build" src/xmindkeygen/*.java

copy ..\..\bak\*.jar .\build\

cd build
java -classpath "../../../lib/*;../../../bak/*;." xmindKeygen.Main


jar -uf net.xmind.verify_3.5.1.201411201906.jar net/
jar -uf org.xmind.meggy_3.5.1.201411201906.jar org/
cd ..
