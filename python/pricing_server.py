from hashlib import pbkdf2_hmac
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
        if "ford" in request.model.lower():
            result = { 'price' : 100, 'currencyCode' : 'GPB'}
        elif "delorean" in request.model.lower():
            result = { 'price': 50000, 'currencyCode': 'USD' }
        else:
            result = { 'price': 1000, 'currencyCode': 'EUR' }
        
        return pb2.PriceReply(**result)


def serve():
    server = grpc.server(futures.ThreadPoolExecutor(max_workers=10))
    pb2_grpc.add_PricerServicer_to_server(PricingService(), server)
    server.add_insecure_port('[::]:5002')

    key = open('d:/dropbox\\workshop.ursatile.com/private.key', 'rb').read()
    crt = open(r'd:\dropbox\workshop.ursatile.com/certificate.crt', 'rb').read()
    server_credentials = grpc.ssl_server_credentials(((key, crt,),))
    server.add_secure_port('[::]:5003', server_credentials)

    server.start()
    print('Autobarn gRPC Pricing Server is running!')
    server.wait_for_termination()

if __name__ == '__main__':
    serve()


