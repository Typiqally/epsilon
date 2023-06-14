<template>
    <a 
        class="btn" 
        href="https://localhost:7084/document/word" 
        style="display: flex; justify-content: space-between">
        <img 
            width="20" 
            height="20" 
            src="@/assets/download-icon.png" 
            alt="download-button-icon"/> 
        Download document
    </a>
</template>

<style>
img {
    margin-right: 8px;
    /* This makes the PNG black by default */
    filter: brightness(1) invert(0);
}

.btn {
    color: black;
    background-color: #f2f3f8;
    text-align: center;
    padding: 1rem;
    border-radius: 9px;
}

.btn:hover {        
    color: white;
    background-color: #848da4;
}

.btn:hover img {
    /* This changes the PNG to white on hover of the button */
    filter: brightness(0) invert(1);
}
</style>
