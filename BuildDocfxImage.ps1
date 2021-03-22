$version = minver
docfx metadata docfx/docfx.json
docker build -t cuemon-docfx:$version  -f Dockerfile.docfx .
get-childItem -recurse -path docfx/api -include *.yml, .manifest | remove-item
docker tag cuemon-docfx:$version tcr.cuemon.dk/cuemon-docfx:$version
docker push tcr.cuemon.dk/cuemon-docfx:$version