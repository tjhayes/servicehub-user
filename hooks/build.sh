#!/bin/bash

git checkout -b master
git fetch origin master

docker image build -f dockerfile -t 
