<template>
  <table class="kpi-legend">
    <tr class="legend__row">
      <td class="kpi-color lvl1" />
      <th class="kpi-text">
        Lvl 1
      </th>
    </tr>
    <tr class="legend__row">
      <td class="kpi-color lvl2" />
      <th class="kpi-text">
        Lvl 2
      </th>
    </tr>
    <tr class="legend__row">
      <td class="kpi-color lvl3" />
      <th class="kpi-text">
        Lvl 3
      </th>
    </tr>
    <tr class="legend__row">
      <td class="kpi-color lvl4" />
      <th class="kpi-text">
        Lvl 4
      </th>
    </tr>
  </table>
</template>

<script lang="ts" setup>

</script>

<style>
.kpi-legend {
    display: flex;
    flex-direction: column;
    width: fit-content;
    height: fit-content;
    padding: 2rem;
}

.kpi-text {
    font-weight: 400;
}

.kpi-color {
    width: 3rem;
}

.lvl1 {
    background-color: #8EAADB;
}

.lvl2 {
    background-color: #A8D08D;
}

.lvl3 {
    background-color: #FFD965;
}

.lvl4 {
    background-color: #B15EB2;
}

.legend__row {
    display: grid;
    grid-template-columns: repeat(2, 5rem);
    padding: 0.5rem;
}
</style>
