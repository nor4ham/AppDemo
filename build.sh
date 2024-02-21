#!/bin/bash

# Variables
dbsecret="db-password-secret"
namespace="app"
# End Variables

# create the cluster
echo "--------------------Creating ack--------------------"

cd terraform-v1 
terraform init 
terraform apply -auto-approve
cd ..


 # after Connect to the cluster

 # ......

# create namespace
echo "--------------------creating Namespace--------------------"
kubectl create ns $namespace || true

# Generate database password
echo "--------------------Generate DB password--------------------"
PASSWORD=$(openssl rand -base64 12)

# Store the generated password in k8s secrets
echo "--------------------Store the generated password in k8s secret--------------------"
kubectl create secret generic $dbsecret --from-literal=DB_PASSWORD=$PASSWORD --namespace=$namespace || true

# Deploy the application
echo "--------------------Deploy App--------------------"
kubectl apply -n $namespace -f k8s

# Wait for application to be deployed
echo "--------------------Wait for all pods to be running--------------------"
