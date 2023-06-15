<template>
    <ul class="slider">
        <li class="sliderOne">
            <input
                id="slide1"
                type="radio"
                name="switch_view"
                value="performance"
                :checked="true" />
            <label class="slider-item" for="slide1">
                Performance dashboard
            </label>
        </li>
        <li class="sliderOne">
            <input
                id="slide2"
                type="radio"
                name="switch_view"
                value="competence" />
            <label class="slider-item" for="slide2">
                Competence document
            </label>
        </li>
    </ul>
</template>

<style scoped>
.slider {
    list-style: none;
    background-color: #f2f3f8;
    margin: 2rem 0;
    border-radius: 0.5rem;
    min-height: 40px;
    width: fit-content;
    display: flex;
    align-items: center;
    justify-content: center;
}

.slider-item {
    color: black;
    padding: 6px 1rem;
    margin: 0 3px;
    border-radius: 0.4rem;
    cursor: pointer;
}

input {
    visibility: hidden;
    height: 0;
    width: 0;
    position: absolute;
}

input[type="radio"]:checked + label {
    background-color: white;
}
</style>
