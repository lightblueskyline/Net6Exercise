# 通过 http-repl 测试 WebAPI

```bash
dotnet tool install --global Microsoft.dotnet-httprepl
```

## HttpPost

```bash
httprepl https://localhost:5001/api/todoitems
post -h Content-Type=application/json -c "{"name":"walk dog","isComplete":true}"
```

## HttpGet

```bash
connect https://localhost:5001/api/todoitems/1
get
#
connect https://localhost:5001/api/todoitems
get
```

## HttpPut

```bash
connect https://localhost:5001/api/todoitems/1
put -h Content-Type=application/json -c "{"id":1,"name":"feed fish","isComplete":true}"
```

## HttpDelete

```bash
connect https://localhost:5001/api/todoitems/1
delete
```
