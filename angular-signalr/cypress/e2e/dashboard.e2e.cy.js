describe('dashboard e2e specs', () => {
  it('should login and take user to devices dashboard page', () => {
    cy.login('host','user','pwd');

    cy.get('app-device-view').should('exist');

    cy.location().should((location) => {
        expect(location.href).should('contain','/device-status');
    })

  });

  it('should logout user', () => {

    cy.login('host','user','pwd');

    cy.visit(`http://localhost:4200/zone-status`).then(() => {
        cy.logout();
        cy.location().should((location) => {
          expect(location.href).to.eq('http://localhost:4200/login');
        });
    });

  });

})


