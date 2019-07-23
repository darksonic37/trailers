#! /bin/bash

set -euxo pipefail

docker build --rm -t trailers-server server
docker build --rm -t trailers-cache cache
docker build --rm -t trailers-client client
