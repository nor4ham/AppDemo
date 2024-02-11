# AppDemo
### minikube start --driver=docker --cpus 4 --memory 8880 
### kubectl create ns app
### kubectl create secret -n app  generic db-password-secret --from-literal Password=postgres
### kubectl apply -n app -f k8s/ 
### kubectl get svc -n app 
### docker buildx build --platform linux/arm64 -t ghcr.io/nor4ham/appdemo:13-arm64 . --push
### minikube service  -n app app-service --url                                                 