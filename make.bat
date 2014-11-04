copy .\bak\net.xmind.verify_3.5.0.201410310637.jar .\build\
copy .\bak\org.xmind.meggy_3.5.0.201410310637.jar .\build\

cd build
jar -uf net.xmind.verify_3.5.0.201410310637.jar net/
jar -uf org.xmind.meggy_3.5.0.201410310637.jar org/