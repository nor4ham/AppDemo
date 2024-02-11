#!/bin/bash

# Variables
cluster_name=""
region="" #Make sure it is the same in the terraform variables
alibaba_id=""
repo_name="appdemo"
domain=""
dbsecret="db-password-secret"
namespace="app"
# End Variables



# create namespace
echo "--------------------creating Namespace--------------------"
kubectl create ns $namespace || true

# Generate database password
echo "--------------------Generate DB password--------------------"
PASSWORD=$(openssl rand -base64 12)

# Store the generated password in k8s secrets
echo "--------------------Store the generated password in k8s secret--------------------"
kubectl create secret generic $dbsecret --from-literal=Password=$PASSWORD --namespace=$namespace || true

# Deploy the application
echo "--------------------Deploy App--------------------"
cd Desktop/dotnet/AppDemo/
kubectl apply -n $namespace -f k8s




