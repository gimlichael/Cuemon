#!/bin/sh

echo "Updating OpenSSL to be backward compatible ..."
wget http://security.ubuntu.com/ubuntu/pool/main/o/openssl/libssl1.1_1.1.1f-1ubuntu2.16_amd64.deb 
sudo dpkg -i libssl1.1_1.1.1f-1ubuntu2.16_amd64.deb && rm libssl1.1_1.1.1f-1ubuntu2.16_amd64.deb
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

echo "Installing PWSH ..."

# Update the list of packages
sudo apt-get update
# Install pre-requisite packages.
sudo apt-get install -y wget apt-transport-https software-properties-common
# Download the Microsoft repository GPG keys
wget -q "https://packages.microsoft.com/config/ubuntu/$(lsb_release -rs)/packages-microsoft-prod.deb"
# Register the Microsoft repository GPG keys
sudo dpkg -i packages-microsoft-prod.deb
# Update the list of packages after we added packages.microsoft.com
sudo apt-get update
# Install PowerShell
sudo apt-get install -y powershell

echo "Installing MONO  ..."

sudo apt-get install gnupg ca-certificates
sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF
echo "deb https://download.mono-project.com/repo/ubuntu stable-focal main" | sudo tee /etc/apt/sources.list.d/mono-official-stable.list
sudo apt-get update
sudo apt-get install -y mono-devel

echo "Installing .NET  ..."

sudo curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin -channel LTS --install-dir /usr/share/dotnet
sudo chmod -R 0775 /usr/share/dotnet
dotnet --info