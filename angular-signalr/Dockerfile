FROM node:latest as build

WORKDIR /app

RUN apt update && apt -y install gettext-base

COPY package.json ./

RUN npm install

COPY . .

ARG API_URL

RUN chmod +x ./changeEnvVariable.sh

RUN ./changeEnvVariable.sh

RUN npm run build

FROM nginx:latest

COPY --from=build /app/dist/angular-signalr-demo/browser /usr/share/nginx/html

RUN chmod 755 /usr/share/nginx/html/assets/*

