$version = minver
docfx metadata docfx.json
curl -sSL --output cuemon-xrefmap.yml https://docs.cuemon.net/xrefmap.yml
(echo "baseUrl: https://docs.cuemon.net/" && cat cuemon-xrefmap.yml) > cuemon-xrefmap.lock && mv cuemon-xrefmap.lock xrefmaps/cuemon-xrefmap.yml
docker build -t cuemon-docfx:$version -f Dockerfile.docfx .
get-childItem -recurse -path api -include *.yml, .manifest | remove-item
get-childItem -recurse -path xrefmaps -include *.yml, .manifest | remove-item
remove-Item cuemon-xrefmap.yml