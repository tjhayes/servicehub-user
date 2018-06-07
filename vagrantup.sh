#!/bin/bash

# MACHINE - PACKAGE VERSIONS
DOCKER_CE=18.03.1~ce-0~ubuntu
TERRAFORM=0.11.7

# MACHINE - PRE-DEFINITION
apt update
apt install -y \
  apt-transport-https \
  ca-certificates \
  curl \
  python3 \
  python3-pip \
  software-properties-common \
  unzip

# MACHINE - AWS CLI
pip3 install --upgrade awscli

# MACHINE - AZURE CLI
AZ_REPO=$(lsb_release -cs)
echo "deb [arch=amd64] https://packages.microsoft.com/repos/azure-cli/ $AZ_REPO main" | \
     sudo tee /etc/apt/sources.list.d/azure-cli.list

apt-key adv --keyserver packages.microsoft.com --recv-keys 52E16F86FEE04B979B07E28DB02C46DF417A0893
curl -L https://packages.microsoft.com/keys/microsoft.asc | sudo apt-key add -

apt update
apt install -y \
  azure-cli

# MACHINE - DOCKER CE
curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo apt-key add -
add-apt-repository \
  "deb [arch=amd64] https://download.docker.com/linux/ubuntu \
  $(lsb_release -cs) stable"

apt update
apt install -y \
  docker-ce=$DOCKER_CE

usermod -aG docker vagrant

# MACHINE - TERRAFORM
wget -q https://releases.hashicorp.com/terraform/$TERRAFORM/terraform_${TERRAFORM}_linux_amd64.zip
unzip terraform_${TERRAFORM}_linux_amd64.zip
mv terraform /usr/local/bin
rm terraform_${TERRAFORM}_linux_amd64.zip

# MACHINE - POST-DEFINITION
shutdown 0
