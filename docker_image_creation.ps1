Copy-Item "src/Services/Auth/Auth.API/Dockerfile" -Destination "Dockerfile"
docker image build -t k8s_auth .
Remove-Item "Dockerfile"

Copy-Item "src/Services/Cocktails/Cocktails.API/Dockerfile" -Destination "Dockerfile"
docker image build -t k8s_cocktail .
Remove-Item "Dockerfile"

Copy-Item "src/Services/Orders/Orders.API/Dockerfile" -Destination "Dockerfile"
docker image build -t k8s_order .
Remove-Item "Dockerfile"

docker run -p 8000:8080 --detach --name k8s_auth_container k8s_auth

docker run -p 4002:8080 --detach --name k8s_cocktail_container k8s_cocktail

docker run -p 3500:8080 --detach --name k8s_order_container k8s_order