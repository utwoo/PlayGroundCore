
#### Install Consule 
```bash
docker run -d -p 8500:8500 -p 8600:8600/udp --name=consul consul agent -server -ui -node=server -bootstrap-expect=1 -client=0.0.0.0
```

#### Get Value by registing configuration in Consul (Hot Update)
Use *Winton.Extensions.Configuration.Consul* package to get value by configuration.
[Winton.Extensions.Configuration.Consul](https://github.com/wintoncode/Winton.Extensions.Configuration.Consul)