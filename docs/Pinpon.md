# Pinpon

Pour se connecter à l'interface, il faut aller à l'adresse <http://10.30.2.7/>

Une fois l'interface chargée, plusieurs écrans sont à votre disposition afin d'administrer celui-ci.

## Home

Sur la page d'accueil se trouve le bouton qui est sans aucun doute le plus important !

![Queues pages](./images/pinpon-front-home.png)

## Queue

La page Queue permet d'ajouter des Queues à poller pour le pinpon. Celle-ci permet donc :

- d'ajouter des queues
- de **désactiver** des queues

![Queues pages](./images/pinpon-front-queue.png)

### Add new queue to poll

Pour ajouter une nouvelle queue à poller, il faut :

- créer une nouveau service bus sur Azure

![Create a new Service bus](./images/azure-service_bus-creation.png)
![Create a new Service bus #2](./images/azure-service_bus-creation2.png)

- créer un nouveau Topic (si ce n'est pas possible il faut d'abord aller sur `scale > Messaging tier > Standard`)
  - **NB: il faut se garder le nom du topic pour la suite**

![Create a new Topic](./images/azure-service_bus-topic_creation.png)

- créer une nouvelle subscription
  - **NB: il faut se garder le nom de la subscription pour la suite**

![Create a new subscription](./images/azure-service_bus-subscription_creation.png)

- créer une nouvelle paire de clés d'accès en **lecture** pour le **pinpon**
  - **NB: il faut se garder une des deux Connection String pour la suite**
  - **NB2: ⚠ Il ne faut pas inclure la variable `EntityPath=` de la connection string**

![Create a new subscription](./images/azure-service_bus-share_key_creation.png)

- Pour la clé SAS utilisé par les pipelines de build, pour envoyer les events, il faut récupérer la clé principale du Service Bus [cf doc](./VSTS.md)

![Retrieve Write Access Key](./images/azure-service_bus-share_key_for_VSTS.png)

- c'est tout pour la partie Azure, il faut maintenant se rendre sur la page d'administration du pinpon <http://10.30.2.7/queue> et rajouter la nouvelle queue avec les paramètres récupérés aux étapes précédentes

![Add a queue](./images/pinpon-front-add_queue.png)
