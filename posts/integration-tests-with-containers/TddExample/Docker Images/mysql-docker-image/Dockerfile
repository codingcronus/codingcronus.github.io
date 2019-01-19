FROM zanox/mysql

EXPOSE 3306

COPY schema.sql /
RUN start-mysql && \
    mysql < /schema.sql && \
    stop-mysql