


$(function () {
    
    var messenger = $.connection.messenger; // generate the client-side hub proxy { Initialized to Exposed Hub }


    function init() {

        messenger.server.addToGroup("sourcing");
        
        return messenger.server.getAllMessages().done(function(message) {
            // Process Message Indivudally if Necessary
            publish(message);
        });
    }

    messenger.begin = function() {
        //$.jGrowl.defaults.animateClose = { width: 'hide' };
        //$.jGrowl("Messenging System Started", { life: 500 });
        toastr.info("Messenging System Started");
    };

    function publish(message) {
        if (message.IsWarning) {
            toastr.warning(message.Content);
        }

        if (message.IsInformation) {
            toastr.info(message.Content);
        }

        if (message.IsError) {
            toastr.error(message.Content);
        }
        
    }

    messenger.client.broadcastMessage = function(name,message) {
        /* $.jGrowl(message.Content, { header: message.Title, sticky: false });*/
        //jGrowlTheme('mono', '<span class="' + "message" + '">' + message.Title + "</span>", '<span class="' + "message" + '">' + message.Content + "</span>", 'images/angryjohn.jpg');
        //http://www.webstuffshare.com/demo/jGrowl-Theme/images/messi.jpg
        publish(message);

    };


    // Start the Connection
    $.connection.hub.start().done(function () {
        
        init();
        
        messenger.begin();

        messenger.server.heartbeat("from javascript");

    });


});