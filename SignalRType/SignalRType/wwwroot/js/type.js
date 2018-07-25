
var prompt = "This is a test to see who can type the fastest.";

// create SignalR connection
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/typeHub")
    .build();

connection.on("LoadPlayers", players => {
    console.log(players);
    players.map(p => {
        createPlayer(p.name, p.connectionId, false);
    });
});

connection.on("RecieveNewPlayer", (user, id, isUser) => {
    createPlayer(user, id, isUser);
});

connection.on("RecieveProgress", (player, progress, text, isUser) => {
    var { connectionId, name } = player;
    var input = document.getElementById(connectionId).querySelector("input");
    var progressBar = document.getElementById(connectionId).querySelector(".progress-bar");

    var pct = (text.length / prompt.length) * 100;

    progressBar.style.width = pct += "%";

    var inputText = text;
 
    if (!isUser) {
        input.value = text;
    }

    if (inputText.length == prompt.length) {
        if (inputText == prompt) {
            progressBar.style.backgroundColor = "#05b919";
            var winMessage = `${name} WINS!`;
            document.getElementById("message").innerHTML = winMessage.toUpperCase();
        } else {
            progressBar.style.backgroundColor = "#dc1d1d";
        }
    }
});

connection.on("UpdateStartStatus", (numInGame, numReady) => {
    if (numReady < numInGame)
    {
        document.getElementById("status").innerHTML = `${numInGame} players have joined. ${numReady}/${numInGame} are ready to start.`;
    }
    else if (numInGame == numReady)
    {
        document.getElementById("status").innerHTML = `All players are ready to start. Prepare to type!`;
        setTimeout(() => {
            document.getElementById("prompt").innerHTML = prompt;
            document.getElementById("status").style.visibility = "hidden";
        }, 3000);
    }
});


connection.start().catch(err => console.error(err.toString()));


document.getElementById("joinButton").addEventListener("click", e => {
    console.log("Sending new player")
    const user = document.getElementById("userInput").value;
    connection.invoke("SendNewPlayer", user);
    e.preventDefault();
});

document.getElementById("readyButton").addEventListener("click", e => {
    connection.invoke("SendStartStatus")
    document.getElementById("readyButton").style.display = "none";
});


function createPlayer(user, id, isUser) {
    console.log("Incoming new player");

    // create clone of player HTML template
    var player = document.getElementById("template").cloneNode(true);

    // display player username and set id
    player.querySelector("#userName").innerHTML = user;
    player.id = id;
    player.style.display = "block";

    // get player input
    var input = player.querySelector("input");

    if (!isUser) {
        // disable input and add to bottom of player list
        input.disabled = true;
        document.getElementById("players").appendChild(player);
    } else {
        // add to top of player list
        document.getElementById("players").prepend(player);
        document.getElementById("readyButton").style.display = "block";
        document.getElementById("userInput").disabled = true;
        document.getElementById("joinButton").style.display = "none";
        document.getElementById("leaveButton").style.display = "block";
    }

    input.addEventListener("input", e => {
        connection.invoke("SendProgress", input.value.length, input.value);
    });

    console.log("New player created");
}




