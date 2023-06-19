FROM node:16 AS Build

WORKDIR /build
EXPOSE 443

RUN curl -f https://get.pnpm.io/v6.16.js | node - add --global pnpm

# pnpm fetch does require only lockfile
COPY pnpm-lock.yaml ./

ADD . ./
RUN pnpm install
RUN pnpm run build


FROM httpd:2.4 AS Host

COPY --from=Build /build/dist/ /usr/local/apache2/htdocs/