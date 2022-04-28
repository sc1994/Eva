# 使用Dapr实现的任务管理系统

### 基本环境配置
配置根目录下 .env 文件内容即可

### 如何启动

整体分为2部分

1. 启动基础服务
    
```shell
docker-compose -f docker-compose.base.yml -p eva_base up -d
```

2. 启动业务服务

```bash
docker-compose -f docker-compose.yml -f docker-compose.test.yml up -d
```


### 数据初始化

找到根目录下的sql文件初始化到mysql中即可

### 服务

- [配置中心](http://localhost:5005/ui#/home)  admin@Password@1
- [zipkin](http://localhost:5411/zipkin)
- [seq](http://localhost:5340/#/events)















