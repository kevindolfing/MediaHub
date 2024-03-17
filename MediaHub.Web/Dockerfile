### STAGE 1:BUILD ###
# Defining a node image to be used as giving it an alias of "build"
# Which version of Node image to use depends on project dependencies
# This is needed to build and compile our code
# while generating the docker image
FROM node:latest AS build
# Create a Virtual directory inside the docker image
WORKDIR /dist/src/app

COPY . .

# Install the node modules
RUN npm install

# Build the angular app
RUN npm run build

FROM nginx:1.15 AS ngi
COPY --from=build /dist/src/app/dist/media-hub.web/browser /usr/share/nginx/html
#COPY /nginx.conf  /etc/nginx/conf.d/default.conf

EXPOSE 80
