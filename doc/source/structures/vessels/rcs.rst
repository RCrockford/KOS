.. _rcs:

RCS
======

Some of the Parts returned by :ref:`LIST PARTS <list command>` will be of type RCS. It is also possible to get just the RCS parts by executing ``LIST RCS``, for example::

    LIST RCS IN myVariable.
    FOR rcs IN myVariable {
        print "An rcs thruster exists with ISP = " + rcs:ISP.
    }.

.. structure:: RCS

    .. list-table::
        :header-rows: 1
        :widths: 1 1 2

        * - Suffix
          - Type (units)
          - Description

        * - All suffixes of :struct:`Part`
          -
          - :struct:`RCS` objects are a type of :struct:`Part`


            AddSuffix("ENABLE", new NoArgsVoidSuffix(Enable));
        * - :meth:`ENABLE`
          -
          - Turn thruster on
        * - :meth:`DISABLE`
          -
          - Turn thruster off
        * - :meth:`ENABLEYAW`
          -
          - Enable yaw control for this thruster.
        * - :meth:`DISABLEYAW`
          -
          - Disable yaw control for this thruster.
        * - :meth:`ENABLEPITCH`
          -
          - Enable pitch control for this thruster.
        * - :meth:`DISABLEPITCH`
          -
          - Disable pitch control for this thruster.
        * - :meth:`ENABLEROLL`
          -
          - Enable roll control for this thruster.
        * - :meth:`DISABLEROLL`
          -
          - Disable roll control for this thruster.
        * - :meth:`ENABLEFORE`
          -
          - Enable fore/aft control for this thruster.
        * - :meth:`DISABLEFORE`
          -
          - Disable fore/aft control for this thruster.
        * - :meth:`ENABLESTARBOARD`
          -
          - Enable port/starboard control for this thruster.
        * - :meth:`DISABLESTARBOARD`
          -
          - Disable port/starboard control for this thruster.
        * - :meth:`ENABLETOP`
          -
          - Enable dorsal/ventral control for this thruster.
        * - :meth:`DISABLETOP`
          -
          - Disable dorsal/ventral control for this thruster.
        * - :attr:`THRUSTLIMIT`
          - :ref:`scalar <scalar>` (%)
          - Tweaked thrust limit.
        * - :attr:`MAXTHRUST`
          - :ref:`scalar <scalar>` (kN)
          - Untweaked thrust limit.
        * - :meth:`MAXTHRUSTAT(pressure)`
          - :ref:`scalar <scalar>` (kN)
          - Max thrust at the specified pressure (in standard Kerbin atmospheres).
        * - :attr:`THRUST`
          - :ref:`scalar <scalar>` (kN)
          - Current thrust. Zero if rcs disabled.
        * - :attr:`AVAILABLETHRUST`
          - :ref:`scalar <scalar>` (kN)
          - Available thrust at full throttle accounting for thrust limiter.
        * - :meth:`AVAILABLETHRUSTAT(pressure)`
          - :ref:`scalar <scalar>` (kN)
          - Available thrust at the specified pressure (in standard Kerbin atmospheres).
        * - :attr:`FUELFLOW`
          - :ref:`scalar <scalar>` (l/s)
          - Rate of fuel burn
        * - :attr:`ISP`
          - :ref:`scalar <scalar>`
          - `Specific impulse <http://wiki.kerbalspaceprogram.com/wiki/Specific_impulse>`_
        * - :meth:`ISPAT(pressure)`
          - :ref:`scalar <scalar>`
          - `Specific impulse <http://wiki.kerbalspaceprogram.com/wiki/Specific_impulse>`_ at the given pressure (in standard Kerbin atmospheres).
        * - :attr:`VACUUMISP`
          - :ref:`scalar <scalar>`
          - Vacuum `specific impulse <http://wiki.kerbalspaceprogram.com/wiki/Specific_impulse>`_
        * - :attr:`VISP`
          - :ref:`scalar <scalar>`
          - Synonym for VACUUMISP
        * - :attr:`SEALEVELISP`
          - :ref:`scalar <scalar>`
          - `Specific impulse <http://wiki.kerbalspaceprogram.com/wiki/Specific_impulse>`_ at Kerbin sealevel
        * - :attr:`SLISP`
          - :ref:`scalar <scalar>`
          - Synonym for SEALEVELISP
        * - :attr:`FLAMEOUT`
          - :ref:`Boolean <boolean>`
          - Check if no more fuel.
        * - :attr:`THRUSTVECTORS`
          - :struct:`List`
          - List of thrust :struct:`Vectors <Vector>` for this RCS module.


.. note::

    A :struct:`RCS` is a type of :struct:`Part`, and therefore can use all the suffixes of :struct:`Part`.

.. method:: RCS:ENABLE

    Call to enable the rcs thruster.

.. method:: RCS:DISABLE

    Call to disable the rcs thruster.

.. method:: RCS:ENABLEYAW

    Call to enable yaw control for the rcs thruster.

.. method:: RCS:DISABLEYAW

    Call to disable yaw control for the rcs thruster.

.. method:: RCS:ENABLEPITCH

    Call to enable pitch control for the rcs thruster.

.. method:: RCS:DISABLEPITCH

    Call to disable pitch control for the rcs thruster.

.. method:: RCS:ENABLEROLL

    Call to enable roll control for the rcs thruster.

.. method:: RCS:DISABLEROLL

    Call to disable roll control for the rcs thruster.

.. method:: RCS:ENABLEFORE

    Call to enable fore/aft control for the rcs thruster.

.. method:: RCS:DISABLEFORE

    Call to disable fore/aft control for the rcs thruster.

.. method:: RCS:ENABLESTARBOARD

    Call to enable port/starboard control for the rcs thruster.

.. method:: RCS:DISABLESTARBOARD`

    Call to disable port/starboard control for the rcs thruster.

.. method:: RCS:ENABLETOP

    Call to enable dorsal/ventral control for the rcs thruster.

.. method:: RCS:DISABLETOP

    Call to disable dorsal/ventral control for the rcs thruster.

.. attribute:: RCS:THRUSTLIMIT

    :access: Get/Set
    :type: :ref:`scalar <scalar>` (%)

    If this is a thruster with a thrust limiter (tweakable) enabled, what
    percentage is it limited to?  Note that this is expressed as a
    percentage, not a simple 0..1 coefficient.  e.g. To set thrustlimit
    to half, you use a value of 50.0, not 0.5.

    This value is not allowed to go outside the range [0..100].  If you
    attempt to do so, it will be clamped down into the allowed range.

    Note that although a kerboscript is allowed to set the value to a
    very precise number (for example 10.5123), the stock in-game display
    widget that pops up when you right-click the rcs will automatically
    round it to the nearest 0.5 whenever you open the panel.  So if you
    do something like ``set ship:part[20]:thrustlimit to 10.5123.`` in
    your script, then look at the rightclick menu for the rcs, the very
    act of just looking at the menu will cause it to become 10.5 instead
    of 10.5123.  There isn't much that kOS can do to change this.  It's a
    user interface decision baked into the stock game.

.. _rcs_MAXTHRUST:

.. attribute:: RCS:MAXTHRUST

    :access: Get only
    :type: :ref:`scalar <scalar>` (kN)

    How much thrust would this rcs thruster give at its current atmospheric pressure if the throttle was max at 1.0, and the thrust limiter was max at 100%.  Note this might not be the thruster's actual max thrust it could have under other air pressure conditions.  Some thrusters have a very different value for MAXTHRUST in vacuum as opposed to at sea level pressure.

.. _rcs_MAXTHRUSTAT:

.. method:: RCS:MAXTHRUSTAT(pressure)

    :parameter pressure: atmospheric pressure (in standard Kerbin atmospheres)
    :type: :ref:`scalar <scalar>` (kN)

    How much thrust would this rcs thruster give if both the throttle and thrust limtier was max at the given atmospheric pressure.  Use a pressure of 0.0 for vacuum, and 1.0 for sea level (on Kerbin) (or more than 1 for thicker atmospheres like on Eve).
    (Pressure must be greater than or equal to zero.  If you pass in a
    negative value, it will be treated as if you had given a zero instead.)

.. attribute:: RCS:THRUST

    :access: Get only
    :type: :ref:`scalar <scalar>` (kN)

    How much thrust is this rcs thruster is giving at this very moment.

.. _rcs_AVAILABLETHRUST:

.. attribute:: RCS:AVAILABLETHRUST

    :access: Get only
    :type: :ref:`scalar <scalar>` (kN)

    Taking into account the thrust limiter tweakable setting, how much thrust would this rcs thruster give if the throttle was max at its current thrust limit setting and atmospheric pressure conditions.

.. _rcs_AVAILABLETHRUSTAT:

.. method:: RCS:AVAILABLETHRUSTAT(pressure)

    :parameter pressure: atmospheric pressure (in standard Kerbin atmospheres)
    :type: :ref:`scalar <scalar>` (kN)

    Taking into account the thrust limiter tweakable setting, how much thrust would this rcs thruster give if the throttle was max at its current thrust limit setting, but at a different atmospheric pressure you pass into it.  The pressure is measured in ATMs, meaning 0.0 is a vacuum, 1.0 is sea level at Kerbin.
    (Pressure must be greater than or equal to zero.  If you pass in a
    negative value, it will be treated as if you had given a zero instead.)

.. attribute:: RCS:FUELFLOW

    :access: Get only
    :type: :ref:`scalar <scalar>` (Liters/s)

    Rate at which fuel is being burned.

.. attribute:: RCS:ISP

    :access: Get only
    :type: :ref:`scalar <scalar>`

    `Specific impulse <http://wiki.kerbalspaceprogram.com/wiki/Specific_impulse>`_

.. method:: RCS:ISPAT(pressure)

    :parameter pressure: atmospheric pressure (in standard Kerbin atmospheres)
    :type: :ref:`scalar <scalar>`

    `Specific impulse <http://wiki.kerbalspaceprogram.com/wiki/Specific_impulse>`_ at the given atmospheric pressure.  Use a pressure of 0 for vacuum, and 1 for sea level (on Kerbin).
    (Pressure must be greater than or equal to zero.  If you pass in a
    negative value, it will be treated as if you had given a zero instead.)

.. attribute:: RCS:VACUUMISP

    :access: Get only
    :type: :ref:`scalar <scalar>`

    Vacuum `specific impulse <http://wiki.kerbalspaceprogram.com/wiki/Specific_impulse>`_

.. attribute:: RCS:VISP

    :access: Get only
    :type: :ref:`scalar <scalar>`

    Synonym for :VACUUMISP

.. attribute:: RCS:SEALEVELISP

    :access: Get only
    :type: :ref:`scalar <scalar>`

    `Specific impulse <http://wiki.kerbalspaceprogram.com/wiki/Specific_impulse>`_ at Kerbin sealevel.

.. attribute:: RCS:SLISP

    :access: Get only
    :type: :ref:`scalar <scalar>`

    Synonym for :SEALEVELISP

.. attribute:: RCS:FLAMEOUT

    :access: Get only
    :type: :ref:`Boolean <boolean>`

    Is this rcs thruster failed because it is starved of a resource (monopropellant)?

.. attribute:: RCS:THRUSTVECTORS

    :access: Get only
    :type: :struct:`List` of :struct:`Vectors <Vector>`

    This gives a list of all the vectors that this RCS module can thrust along.

