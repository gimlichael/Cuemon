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

sudo apt-get -y install openjdk-17-jdk 

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

echo "Installing NuGet  ..."

sudo apt-get install -y nuget