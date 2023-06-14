<template>
    <a class="btn" href="https://localhost:7084/document/word" style="display: flex; justify-content: space-between;">
        <img width="24" height="24" src="https://img.icons8.com/fluency-systems-filled/48/download.png" alt="download"/> 
        Download document
    </a>
</template>

<style>
    img {
        margin-right: 5px;

        /* This makes the PNG black by default */
        filter: brightness(1) invert(0);
    }

    .btn {
        color: black;
        background-color: #F2F3F8;

        text-align: center;
        
        padding: 1rem;
        border-radius: 9px;
    }

    .btn:hover {        
        color: white;
        background-color: #848DA4;
    }

    .btn:hover img {
        /* This changes the PNG to white on hover of the button */
        filter: brightness(0) invert(1);
    }
</style>