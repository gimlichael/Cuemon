$version = minver -i -t v -v w
docker tag cuemon-docfx:$version jcr.codebelt.net/geekle/cuemon-docfx:$version
docker push jcr.codebelt.net/geekle/cuemon-docfx:$version
