// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    // Get the clickable image element
    var clickChatBot = $('#chatbotImg');
    $('.chatbotSection').hide();
    var userResponse = $("#textData").val()
    var chatBotResponse = "GET DATA FROM CHATBOT";

    // Set up a click event listener on the image
    clickChatBot.on('click', function () {
        // Display the modal
        $('.chatbotSection').toggle();
    });
    $("#microphoneBtn").on("click", function () {
        const y = document.querySelector("textarea#textData");
        let recognition;
        let speech = true;
        let isSpeaking = false;

        if (speech) {
            window.SpeechRecognition = window.webkitSpeechRecognition;
            recognition = new SpeechRecognition();
            recognition.interimResults = true;

            recognition.addEventListener('result', e => {
                const transcript = Array.from(e.results)
                    .map(result => result[0])
                    .map(result => result.transcript)
                    .join(' ');
                y.innerHTML = transcript;

                const isSpeakingNow = transcript.trim() !== '';

                if (isSpeakingNow) {
                    isSpeaking = true;
                }
            });

            recognition.onend = () => {
                if (isSpeaking) {
                    setTimeout(() => {
                        if (!isSpeaking) {
                            recognition.stop();
                        }
                    }, 3000);
                }
            };

            recognition.start();
        }
        
    });

    $("#sendMsgBtn").on('click', function () {
        sendMessage()
    });
    // Trigger sendMessage when the Enter key is pressed in the #textData textarea
    $("#textData").keypress(function (e) {
        if (e.which === 13) {
            sendMessage();
        }
    });

    // Functions
    function sendMessage() {
        getUserResponse();
        createNewMessage();
        $("#textData").val("");
    }

    // Create a new message enter by user
    function createNewMessage() {
        // Create a new message div
        var newMessage = $("<div class='message justify-content-end ms-auto mb-4'>" +
            "<img src='https://cdn-icons-png.flaticon.com/512/1144/1144709.png' id='profileImg' />" +
            "<p class='msg'>" + userResponse + "</p>" + "</div>");
        // append new message after the last message
        $(".chatbotSection .messageSection").append(newMessage);
    }

    // Create a new message return by bot
    function botResponse(data) {
        // Create a new message div
        var newMessage = $("<div class='message ms-auto mb-4'>" +
            "<img src='https://cdn-icons-png.flaticon.com/512/11306/11306080.png' id='profileImg' />" +
            "<p class='msg'>" + data + "</p>" + "</div>");
        // append new message after the last message
        $(".chatbotSection .messageSection").append(newMessage);
    }

    function getUserResponse() {
        userResponse = $("#textData").val()
        console.log(userResponse);
        $.ajax({
            url: '/AIChatBot/ChatwBot',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({ userResponse: userResponse }),
            success: function (data) {
                reply = data.reply //CHAT BOT DATA HERE!!!
                botResponse(reply);
                speak(reply);
                console.log(reply);
            },
            error: function (xhr, status, error) {
                console.error("XHR status:", xhr.status);
                console.error("Status:", status);
                console.error("Error:", error);

                alert("An error occurred. Check the console for details.");
            }
        });
    }
    function TexttoSpeech() {
        const voiceSelect = document.getElementById('voices');
        const synth = window.speechSynthesis;
        const voices = synth.getVoices();

        voices.forEach((voice, index) => {
            const option = document.createElement('option');
            option.value = index;
            option.textContent = voice.name;
            voiceSelect.appendChild(option);
        });
    }
    function speak(textToSpeak) {
        if ('speechSynthesis' in window) {
            const synth = window.speechSynthesis;
            const utterance = new SpeechSynthesisUtterance(textToSpeak);

            // Get available voices
            const voices = synth.getVoices();

            // Set the voice (you can change the index to select a different voice)
            utterance.voice = voices[0];

            // Optional: Set other parameters
            // utterance.rate = 1.0;
            // utterance.pitch = 1.0;

            synth.speak(utterance);
        } else {
            alert("Sorry, your browser doesn't support the Web Speech API.");
        }
    }

});

