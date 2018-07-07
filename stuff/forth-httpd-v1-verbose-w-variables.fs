require unix/socket.fs

create buf 1400 allot
create myserver
create mysocket

: listen-and-accept 
  cr ." listening" myserver @ 128 listen cr ." done listen"
  cr ." accepting" myserver @ accept-socket mysocket ! ." accepted socket" 
;

: response s\" HTTP/1.0 200 Ok\r\nContent-Type: text/html\r\n\r\n<html><head><title>hello</title></head><body><h1>hello world</h1></body></html>\r\n" mysocket @ write-socket ;

: get-request mysocket @ buf 1400 read-socket ;

: httpd (  -- )
  cr ." creating server on 8080" s" localhost" 8080 create-server myserver ! cr ." server created"
  begin
    listen-and-accept
    10 0 do \ 200 ms timeout
      cr i . r@ .
      get-request 
      0> if 255 type response r> 9 >r else ." nada" then
      20 ms 
    loop
    mysocket @ close-socket
  again
;
