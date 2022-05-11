Docker commands and images for Autobarn workshop

The commands to build the Docker image are in runme.bat / runme.sh

To capture the running container as an image:

docker commit autobarn-mssql2019-container
sha256:ac4b4dc0134df9b09855de62c85de63ed8635397ec64e6f523d8cd00795e023

docker images
REPOSITORY                       TAG                         IMAGE ID       CREATED          SIZE
<none>                           <none>                      ac4b4dc0134d   32 seconds ago   1.67GB
autobarn-mssql2019-image         latest                      4ab7dfc7fae5   3 minutes ago    1.52GB
ursatile/ursatile-workshops      autobarn-mssql2019-latest   6e4377540070   4 days ago       1.53GB
<none>                           <none>                      2ed913a5ccec   4 days ago       1.52GB
<none>                           <none>                      b79043bb96f3   4 days ago       1.49GB
mcr.microsoft.com/mssql/server   2019-CU10-ubuntu-20.04      62c72d863950   6 weeks ago      1.49GB
mcr.microsoft.com/mssql/server   2019-latest                 5ced205176bc   7 months ago     1.43GB

(look for the <none> <none> created 30-odd seconds ago!)

docker tag ac4b4dc0134d autobarn-mssql2019

docker tag autobarn-mssql2019:latest ursatile/ursatile-workshops:autobarn-mssql2019-latest
docker push ursatile/ursatile-workshops:autobarn-mssql2019-latest