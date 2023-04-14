<template>
  <table class="competence-profile-legend">
    <tr class="competence-profile-legend-row">
      <td class="kpi-color mastery-level-1"/>
      <th class="kpi-text">
        Lvl 1
      </th>
    </tr>
    <tr class="competence-profile-legend-row">
      <td class="kpi-color mastery-level-2"/>
      <th class="kpi-text">
        Lvl 2
      </th>
    </tr>
    <tr class="competence-profile-legend-row">
      <td class="kpi-color mastery-level-3"/>
      <th class="kpi-text">
        Lvl 3
      </th>
    </tr>
    <tr class="competence-profile-legend-row">
      <td class="kpi-color mastery-level-4"/>
      <th class="kpi-text">
        Lvl 4
      </th>
    </tr>
  </table>
</template>

<style>
.competence-profile-legend {
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

.mastery-level-1 {
    background-color: #8EAADB;
}

.mastery-level-2 {
    background-color: #A8D08D;
}

.mastery-level-3 {
    background-color: #FFD965;
}

.mastery-level-4 {
    background-color: #B15EB2;
}

.competence-profile-legend-row {
    display: grid;
    grid-template-columns: repeat(2, 5rem);
    padding: 0.5rem;
}
</style>
