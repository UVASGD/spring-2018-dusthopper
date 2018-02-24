using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface AsteroidInterface {

    // Function called to generate asteroid data on the surface
    void Generate(int asteroidNum);

    void InitDefault();
}
