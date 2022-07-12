$version = minver
docker tag cuemon-docfx:$version codebelt.jfrog.io/geekle/cuemon-docfx:$version
docker push codebelt.jfrog.io/geekle/cuemon-docfx:$version