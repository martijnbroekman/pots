set -e

rm -rf ./pots/bin
dotnet build --configuration Release
dotnet publish --configuration Release
