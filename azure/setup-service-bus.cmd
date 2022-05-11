call az account set --subscription "172d74c0-182e-4951-93fa-c451f1c945e5"
set RG=ursatile-workshops-resource-group
set NS=autobarn-namespace
call az servicebus namespace authorization-rule keys list --resource-group %RG% --namespace-name %NS% --name RootManageSharedAccessKey --query primaryConnectionString --output tsv    

call az servicebus namespace delete --resource-group %RG% --name %NS%
call az servicebus namespace create --resource-group %RG% --name %NS%
call az servicebus topic create --resource-group %RG% --namespace-name %NS% --name autobarn-new-vehicle-topic
call az servicebus topic subscription create --name autobarn-auditlog-subscription --resource-group %RG% --namespace-name %NS% --topic-name autobarn-new-vehicle-topic 
call az servicebus topic subscription create --name autobarn-notifier-subscription --resource-group %RG% --namespace-name %NS% --topic-name autobarn-new-vehicle-topic 

