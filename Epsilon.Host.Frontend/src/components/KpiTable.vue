<template>
    {{hboIDomain}}
  <table>
    <thead>
      <tr>
        <td />
        <th>Analyse</th>
        <th>Advise</th>
        <th>Design</th>
        <th>Realise</th>
        <th>Manage & Control</th>
      </tr>
    </thead>
    <tbody>
      <tr>
        <th>User interaction</th>
        <td />
        <td />
        <td />
        <td />
        <td />
      </tr>
      <tr>
        <th>Infrastructure</th>
        <td />
        <td />
        <td />
        <td />
        <td />
      </tr>
      <tr>
        <th>Organisational processes</th>
        <td />
        <td />
        <td />
        <td />
        <td />
      </tr>
      <tr>
        <th>Software</th>
        <td />
        <td />
        <td />
        <td />
        <td />
      </tr>
      <tr>
        <th>Hardware interfacing</th>
        <td />
        <td />
        <td />
        <td />
        <td />
      </tr>
    </tbody>
  </table>
</template>

<script>
export default {
    name: "KpiTable",
    props:{
        hboIDomain: {
            default: {}
        }
    }
}
</script>

<style>
tbody tr th {
    text-align: left;
}

tbody tr td {
    padding: 8px;
    min-width: 100px;
}

tbody tr {
    border-bottom: 2px solid black;
}

tbody td {
    border: 2px solid black;
}

table {
    border-collapse: collapse;
}

thead th {

}
</style>
