
const firebaseAppScript = document.createElement('script');
firebaseAppScript.src = 'https://www.gstatic.com/firebasejs/8.6.8/firebase-app.js';
document.head.appendChild(firebaseAppScript);

const firebaseStorageScript = document.createElement('script');
firebaseStorageScript.src = 'https://www.gstatic.com/firebasejs/8.6.8/firebase-storage.js';
document.head.appendChild(firebaseStorageScript);
const video = document.getElementById('video');
const face_button = document.getElementById('capture_face');
const path = '../models';
const captchaCode = generateCaptcha();
const video_instructions = document.getElementById('video_instructions');
const username = document.getElementById('sign-up_username')



const promiseAll = Promise.all([
    faceapi.nets.tinyFaceDetector.loadFromUri(path),
    faceapi.nets.faceLandmark68Net.loadFromUri(path),
    faceapi.nets.faceRecognitionNet.loadFromUri(path),
    faceapi.nets.faceExpressionNet.loadFromUri(path)
]);
document.addEventListener('DOMContentLoaded', function () {
    sessionStorage.setItem('captchaCode', captchaCode);
    Promise.all([
        new Promise((resolve) => { firebaseAppScript.onload = resolve; }),
        new Promise((resolve) => { firebaseStorageScript.onload = resolve; })
    ]).then(() => {
        // Initialize Firebase here
        const firebaseConfig = {
            apiKey: "AIzaSyDEjaGgwQaFFDoF_83ynXlLJ87R7LzHBAg",
            authDomain: "pfd-user-image.firebaseapp.com",
            projectId: "pfd-user-image",
            storageBucket: "pfd-user-image.appspot.com",
            messagingSenderId: "431415465527",
            appId: "1:431415465527:web:4f574bb5d5bda9394411b2",
            measurementId: "G-3L6N0M0WR0"
        };

        firebase.initializeApp(firebaseConfig);

        // Now you can use firebase storage and other features
        const storage = firebase.storage();
        // ... rest of your code
        face_button.onclick = function () {
            console.log("Button clicked");
            startVideo();
        }

    });
  
});

function startVideo() {
    navigator.getUserMedia(
        { video: {} },
        (stream) => {
            video.srcObject = stream;
            video.play(); // Start playing the video
        },
        (err) => console.error(err)
    );

    video.addEventListener('loadedmetadata', () => {
        // Video has loaded metadata, now we can create the canvas

        const canvas = faceapi.createCanvasFromMedia(video);

        document.body.append(canvas);
        const displaySize = { width: video.width, height: video.height };
        faceapi.matchDimensions(canvas, displaySize);

        let isFaceDetected = false; // Flag to track face detection

        setInterval(async () => {
            const detections = await faceapi
                .detectAllFaces(video, new faceapi.TinyFaceDetectorOptions())
                .withFaceLandmarks()
                .withFaceExpressions();
            const resizedDetections = faceapi.resizeResults(
                detections,
                displaySize
            );
            canvas.getContext('2d').clearRect(0, 0, canvas.width, canvas.height);


            // Check if any face is detected

            if (detections.length > 0) {
                if (detections[0]["detection"]["_score"] >= 0.90) {
                    if (!isFaceDetected) {
                        isFaceDetected = true;
                        video_instructions.textContent = ""
                        video.pause(); // Pause the video when a face is detected
                        captureScreenshot();

                    }
                }

               
                
            } else {
                isFaceDetected = false;
                video_instructions.textContent = "Position yourself to the center of the screen"
            }
            
        }, 100);
    });
}

function generateCaptcha() {
    const captchaCode = Math.random().toString(36).slice(2, 8).toUpperCase();
    const captchaImage = `https://dummyimage.com/120x40/000/fff&text=${captchaCode}`;

    document.getElementById('captcha-image').src = captchaImage;
    return captchaCode;
}

// Function to validate CAPTCHA
function validateCaptcha() {
    const userInput = document.getElementById('captcha-input').value.toUpperCase();
    const captchaCode = sessionStorage.getItem('captchaCode');

    if (userInput === captchaCode) {
        alert('CAPTCHA validation successful!');
        // You can add further actions here, like submitting a form or enabling a button
    } else {
        alert('CAPTCHA validation failed. Please try again.');
        // Regenerate CAPTCHA on failure
        generateCaptcha();
    }
}
function captureScreenshot() {
    const canvas = document.createElement('canvas');
    canvas.width = video.videoWidth;
    canvas.height = video.videoHeight;
    const ctx = canvas.getContext('2d');
    ctx.drawImage(video, 0, 0, canvas.width, canvas.height);

    // Convert the canvas content to a data URL representing a PNG file
    const dataURL = canvas.toDataURL('image/png');

    // Convert the data URL to a Blob
    const blob = dataURItoBlob(dataURL);

    // Upload the Blob to Firebase
    uploadImageToFirebase(blob);
}



function dataURItoBlob(dataURI) {
    const byteString = atob(dataURI.split(',')[1]);
    const mimeString = dataURI.split(',')[0].split(':')[1].split(';')[0];
    const ab = new ArrayBuffer(byteString.length);
    const ia = new Uint8Array(ab);

    for (let i = 0; i < byteString.length; i++) {
        ia[i] = byteString.charCodeAt(i);
    }

    return new Blob([ab], { type: mimeString });
}

function uploadImageToFirebase(blob) {
    const storage = firebase.storage();
    const storageRef = storage.ref().child("myimages");
    const fileName = username.value + '.png';
    const folderRef = storageRef.child(fileName);
    const uploadTask = folderRef.put(blob);

    uploadTask.on(
        "state_changed",
        (snapshot) => {
            // Handle upload progress if needed
            const progress = (snapshot.bytesTransferred / snapshot.totalBytes) * 100;
            console.log("Upload Progress: " + progress + "%");
        },
        (error) => {
            console.error("Error uploading image to Firebase:", error);
        },
        () => {
            // Image uploaded successfully, get download URL
            storage.ref("myimages").child(fileName).getDownloadURL()
                .then((url) => {
                    console.log("Image uploaded to Firebase. URL:", url);
                    // You can use the URL as needed, for example, display or further processing
                })
                .catch((error) => {
                    console.error("Error getting download URL:", error);
                });
        }
    );
}