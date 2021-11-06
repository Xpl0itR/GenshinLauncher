// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GenshinLauncher;

public record TransferProgress
{
    private readonly long _totalBytes;
    private readonly long _totalBytesTransferred;

    public TransferProgress(long totalBytes, long totalBytesTransferred, double averageBytesPerSecond) =>
        (_totalBytes, _totalBytesTransferred, AverageBytesPerSecond) = (totalBytes, totalBytesTransferred, averageBytesPerSecond);

    public double   AverageBytesPerSecond       { get; }
    public double   FractionCompleted           => (double)_totalBytesTransferred / _totalBytes;
    public TimeSpan EstimatedTimeRemaining      => TimeSpan.FromSeconds((_totalBytes - _totalBytesTransferred) / AverageBytesPerSecond);
    public string   AverageBytesPerSecondString => FormatBytes(AverageBytesPerSecond);
    public string   TotalBytesTransferredString => FormatBytes(_totalBytesTransferred);
    public string   TotalBytesString            => FormatBytes(_totalBytes);

    private static string FormatBytes(double bytes)
    {
        if (bytes == 0)
            return "0B";

        string[] orders = { "B", "KiB", "MiB", "GiB", "TiB", "PiB", "EiB" };
        int      order  = (int)Math.Log(bytes, 1024);

        return $"{bytes / Math.Pow(1024, order):0.#}{orders[order]}";
    }
}

public class TransferProgressTracker : IProgress<long> //TODO: refactor, make thread safe
{
    public event EventHandler<TransferProgress>? ProgressChanged;

    private readonly int                 _numSamples;
    private readonly Queue<(long, long)> _samples;
    private readonly Stopwatch           _tickProvider;
    private readonly long                _totalBytes;

    private long _latestSampledTicks;
    private long _totalBytesTransferred;

    public TransferProgressTracker(long totalBytes, int numSamples)
    {
        _numSamples   = numSamples;
        _totalBytes   = totalBytes;
        _tickProvider = Stopwatch.StartNew();
        _samples      = new Queue<(long, long)>();
    }

    public void Report(long bytesTransferred)
    {
        long elapsedTicks = _tickProvider.ElapsedTicks;
        if (_samples.Count >= _numSamples)
            _samples.Dequeue();
        _samples.Enqueue((_latestSampledTicks = elapsedTicks, _totalBytesTransferred += bytesTransferred));

        if (_samples.Count < 2)
            return;

        (long earliestSampledTicks, long earliestSampledBytesTransferred) = _samples.Peek();
        double speed = (_totalBytesTransferred - earliestSampledBytesTransferred) / ((double)(_latestSampledTicks - earliestSampledTicks) / Stopwatch.Frequency);

        ProgressChanged?.Invoke(this, new TransferProgress(_totalBytes, _totalBytesTransferred, speed));
    }
}