$version = "latest"
$tag = "se22m001/viennarocks:$version"
$buildContext = "."

& docker build --tag $tag $buildContext
& docker push $tag