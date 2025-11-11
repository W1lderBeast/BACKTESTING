window.blazorChartInterop = {
    drawEquityChart: function (canvasId, dates, equityValues, initialCapital) {

        // Convertir les valeurs de capital de string à float/number
        const dataPoints = equityValues.map(s => parseFloat(s));

        const ctx = document.getElementById(canvasId).getContext('2d');

        // Si le graphique existe déjà (pour les lancements multiples), le détruire
        if (window.equityChart) {
            window.equityChart.destroy();
        }

        // Créer l'objet Chart.js
        window.equityChart = new Chart(ctx, {
            type: 'line',
            data: {
                labels: dates,
                datasets: [{
                    label: 'Courbe de Capital',
                    data: dataPoints,
                    borderColor: 'rgba(75, 192, 192, 1)',
                    backgroundColor: 'rgba(75, 192, 192, 0.2)',
                    borderWidth: 2,
                    pointRadius: 0, // Désactiver les points pour une courbe propre
                    tension: 0.1
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: true,
                scales: {
                    x: {
                        title: {
                            display: true,
                            text: 'Date'
                        }
                    },
                    y: {
                        title: {
                            display: true,
                            text: 'Capital (€)'
                        },
                        beginAtZero: false
                    }
                },
                plugins: {
                    legend: {
                        display: true
                    },
                    tooltip: {
                        mode: 'index',
                        intersect: false,
                        callbacks: {
                            // Afficher les valeurs en format monétaire
                            label: function (context) {
                                let label = context.dataset.label || '';
                                if (label) {
                                    label += ': ';
                                }
                                if (context.parsed.y !== null) {
                                    label += new Intl.NumberFormat('fr-FR', { style: 'currency', currency: 'EUR' }).format(context.parsed.y);
                                }
                                return label;
                            }
                        }
                    }
                }
            }
        });
    }
};