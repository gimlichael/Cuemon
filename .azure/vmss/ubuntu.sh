#!/bin/sh

echo "Updating OpenSSL to be backward compatible ..."
wget http://security.ubuntu.com/ubuntu/pool/main/o/openssl/libssl1.1_1.1.1f-1ubuntu2.19_amd64.deb
sudo dpkg -i libssl1.1_1.1.1f-1ubuntu2.19_amd64.deb && rm libssl1.1_1.1.1f-1ubuntu2.19_amd64.deb
sudo sed -i 's/openssl_conf = openssl_init/#openssl_conf = openssl_init/g' /etc/ssl/openssl.cnf

echo "Installing Docker ..."
curl -fsSL https://get.docker.com -o get-docker.sh
sudo sh get-docker.sh

echo "Installing Docker Compose ..."
sudo apt-get update
sudo apt-get install docker-compose-plugin
sudo apt-get  -y install docker-compose

echo "Installing Java Runtime ..."

sudo apt-get  -y install default-jre

echo "Installing MONO  ..."

sudo apt-get install gnupg ca-certificates
sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF
echo "deb https://download.mono-project.com/repo/ubuntu stable-focal main" | sudo tee /etc/apt/sources.list.d/mono-official-stable.list
sudo apt-get update
sudo apt-get install -y mono-devel

echo "Installing .NET  ..."

sudo curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --version 8.0.100-rc.2.23469.4 --install-dir /usr/share/dotnet
sudo cat <<EOF > ~/.bashrc
export PATH=/usr/share/dotnet:$HOME/.dotnet/tools/:$PATH
EOF
sudo chmod -R 777 /usr/share/dotnet