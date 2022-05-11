import grpc
from concurrent import futures
import time
import os
import price_pb2_grpc as pb2_grpc
import price_pb2 as pb2


class PricingService(pb2_grpc.PricerServicer):
    def __init__(self, *args, **kwargs):
        pass

    def GetPrice(self, request, context):
        print(request)
        if "ford" in request.modelCode.lower():
            result = {'price': 5000, 'currencyCode': 'EUR'}
        elif "brown" in request.color.lower():
            result = {'price': 1250, 'currencyCode': 'SEK'}
        elif "delorean" in request.modelCode.lower():
            result = {'price': 50000, 'currencyCode': 'USD'}
        else:
            price = 1000 + (request.year * 10)
            result = {'price': price, 'currencyCode': 'GBP '}

        return pb2.PriceReply(**result)


def serve():
    if os.path.isdir("D:/workshop.ursatile.com"):
        with open('D:/workshop.ursatile.com/workshop.ursatile.com.key', 'rb') as f:
            private_key = f.read()
        with open('D:/workshop.ursatile.com/workshop.ursatile.com.crt', 'rb') as f:
            certificate_chain = f.read()
        server_credentials = grpc.ssl_server_credentials(
            ((private_key, certificate_chain,),))

    server = grpc.server(futures.ThreadPoolExecutor(max_workers=10))
    pb2_grpc.add_PricerServicer_to_server(PricingService(), server)
    server.add_insecure_port('[::]:5002')
    if server_credentials:
        print("Found certificates in D:/workshop.ursatile.com - enabling HTTPS on port 5003")    
        server.add_secure_port('[::]:5003', server_credentials)
    server.start()
    print("Autobarn gRPC Pricing Server running.")
    server.wait_for_termination()

if __name__ == '__main__':
    serve()
