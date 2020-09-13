docfx metadata docfx/docfx.json
docker build -t cuemon-docfx:6.0.0  -f Dockerfile.docfx .
remove-item docfx/obj -recurse
get-childItem -recurse -path docfx/api -include *.yml, .manifest | remove-item