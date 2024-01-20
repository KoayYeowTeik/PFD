//find face and freeze
const video = document.getElementById('video');
const path = '../models';
let imageList = [];

const firebaseAppScript = document.createElement('script');
firebaseAppScript.src = 'https://www.gstatic.com/firebasejs/8.6.8/firebase-app.js';
document.head.appendChild(firebaseAppScript);

const firebaseStorageScript = document.createElement('script');
firebaseStorageScript.src = 'https://www.gstatic.com/firebasejs/8.6.8/firebase-storage.js';
document.head.appendChild(firebaseStorageScript);

// Wait for the scripts to load before using Firebase
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
    imageList = retrieveImages().then(images => {
        imageList = images;
        Promise.all([
            faceapi.nets.tinyFaceDetector.loadFromUri(path),
            faceapi.nets.faceLandmark68Net.loadFromUri(path),
            faceapi.nets.faceRecognitionNet.loadFromUri(path),
            faceapi.nets.faceExpressionNet.loadFromUri(path)
        ]).then(startVideo);    }).catch (error => {
        console.error("Error", error);
    })
 
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
                if (!isFaceDetected) {
                    isFaceDetected = true;
                    video.pause(); // Pause the video when a face is detected
                    const blob = captureScreenshot();
                    findBestMatch(blob, imageList)
                        .then((bestMatch) => {
                            console.log(bestMatch);
                        })
                        .catch((error) => console.error('Error:', error));                   
                }
            } else {
                isFaceDetected = false;
            }
        }, 100);
    });
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

    // Return the Blob
    return blob;
}


// Helper function to convert data URL to Blob
function dataURItoBlob(dataURI) {
    const byteString = atob(dataURI.split(',')[1]);
    const ab = new ArrayBuffer(byteString.length);
    const ia = new Uint8Array(ab);
    for (let i = 0; i < byteString.length; i++) {
        ia[i] = byteString.charCodeAt(i);
    }
    return new Blob([ab], { type: 'image/png' });
}

async function retrieveImages() {
    try {
        let imageList = [];
        const storage = firebase.storage();

        const storageRef = storage.ref('myimages');
        const images = await storageRef.listAll();

        // Fetch and convert each image to blob
        const imagePromises = images.items.map(async (imageRef) => {
            const imageUrl = await imageRef.getDownloadURL();
            const imageName = imageRef.name; // Get the name of the file

            // Fetch the image content as ArrayBuffer
            const arrayBuffer = await fetch(imageUrl).then((res) => res.arrayBuffer());

            // Convert ArrayBuffer to Blob
            const blob = new Blob([arrayBuffer]);

            // Convert the blob to PNG
            const pngBlob = await convertToPNG(blob);

            // Add the PNG blob to the list
            imageList.push({
                blob: pngBlob,
                blobName: imageName, // Append the file name as an attribute
            });

            // Perform face detection and matching
        });

        // Wait for all image promises to resolve
        await Promise.all(imagePromises);

        return imageList;
    } catch (error) {
        console.error('Error retrieving images:', error);
        throw error; // Re-throw the error if needed for error handling in the calling code
    }
}


async function convertToPNG(blob) {
    return new Promise((resolve) => {
        const img = new Image();
        img.onload = () => {
            const canvas = document.createElement('canvas');
            canvas.width = img.width;
            canvas.height = img.height;
            const ctx = canvas.getContext('2d');
            ctx.drawImage(img, 0, 0, img.width, img.height);
            canvas.toBlob(resolve, 'image/png');
        };
        img.src = URL.createObjectURL(blob);
    });
}



async function findBestMatch(queryBlob, DBList) {
    // Load the query image
    const queryImage = await faceapi.bufferToImage(queryBlob);

    // Detect faces in the query image
    const queryFaces = await faceapi.detectAllFaces(queryImage, new faceapi.TinyFaceDetectorOptions()).withFaceLandmarks().withFaceDescriptors();

    // Compute descriptors for faces in the query image
    const queryDescriptors = queryFaces.map((face) => face.descriptor);
    console.log(queryDescriptors);

    // Iterate through the DBList
    let bestMatch = null;
    let bestSimilarity = 0;

    for (const dbBlob of DBList) {
        // Load the DB image
        const dbImage = await faceapi.bufferToImage(dbBlob["blob"]);

        // Detect faces in the DB image
        const dbFaces = await faceapi.detectAllFaces(dbImage, new faceapi.TinyFaceDetectorOptions()).withFaceLandmarks().withFaceDescriptors();

        for (const dbFace of dbFaces) {
            // Check if the descriptor lengths match before computing the match
            if (dbFace.descriptor.length === queryDescriptors[0].length) {

                // Compute Euclidean distance between query and database descriptors
                const distance = faceapi.euclideanDistance(queryDescriptors[0], dbFace.descriptor);
                // Check if the distance is within a threshold (e.g., 0.8)
                if (distance < 0.2) {
                    bestMatch = dbBlob["blobName"].slice(0, -4);
                    return bestMatch
                }
            }
        }
    }

    // Return the best match
}
