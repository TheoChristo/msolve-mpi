#!/bin/bash

IMGNAME="mpi-test"

echo "Building Docker Image " $IMGNAME
docker build -t $IMGNAME -f Dockerfile .
echo "Runing Container"
docker run --rm -it $IMGNAME /bin/bash
echo "Container exited. Removing image"
docker rmi $IMGNAME
