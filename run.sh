#! /bin/bash

./build.sh

docker stack rm trailers

docker stack deploy -c development.yml trailers
